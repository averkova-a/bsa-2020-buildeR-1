﻿using AutoMapper;
using buildeR.BLL.Exceptions;
using buildeR.BLL.Interfaces;
using buildeR.BLL.Services.Abstract;
using buildeR.Common.DTO.BuildHistory;
using buildeR.Common.DTO.BuildStep;
using buildeR.Common.DTO.Project;
using buildeR.DAL.Context;
using buildeR.DAL.Entities;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buildeR.BLL.Services
{
    public sealed class ProjectService : BaseCrudService<Project, ProjectDTO, NewProjectDTO>, IProjectService
    {
        private readonly IQuartzService _quartzService;
        private readonly IBuildStepService _buildStepService;
        public ProjectService(BuilderContext context, IMapper mapper, IQuartzService quartzService, IBuildStepService buildStepService) : base(context, mapper)
        {
            _quartzService = quartzService;
            _buildStepService = buildStepService;
        }

        public override Task<ProjectDTO> GetAsync(int id, bool isNoTracking = false)
        {
            return base.GetAsync(id, isNoTracking);
        }

        public async Task<IEnumerable<ProjectInfoDTO>> GetProjectsByUser(int userId)
        {
           
            var projects = await Context.Projects
                .AsNoTracking()
                .Include(project => project.Owner)
                .Include(project => project.BuildHistories)
                .Where(project => project.OwnerId == userId)
                .ToArrayAsync();
            var projectInfos = Mapper.Map<IEnumerable<ProjectInfoDTO>>(projects);
            return projectInfos;
        }

        public async Task<ProjectDTO> GetProjectByUserId(int userId, int projectId)
        {
            var project = await GetAsync(projectId);
            if (project.OwnerId == userId)
            {
                return project;
            }
            throw new ForbiddenExeption("Read", project.Name, project.Id);
        }
        public async Task<ProjectDTO> CreateProject(NewProjectDTO dto)
        {
            return await base.AddAsync(dto);
        }
        public async Task UpdateProject(ProjectDTO dto, int userId)
        {
            var project = await GetAsync(dto.Id);
            if (project.OwnerId == userId)
            {
                await base.UpdateAsync(dto);
            }
            throw new ForbiddenExeption("Update", project.Name, project.Id);
        }

        public async Task DeleteProject(int id)
        {
            await _quartzService.DeleteAllSheduleJob(id.ToString());
            var project = await GetAsync(id);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), id);
            }
            await base.RemoveAsync(id);     
        }       
        public async Task<ExecutiveBuildDTO> GetExecutiveBuild(int projectId)
        {
            var project = await Context.Projects
                                                .Include(p => p.BuildSteps)
                                                    .ThenInclude(s => s.PluginCommand)
                                                        .ThenInclude(c => c.Plugin)
                                                .Include(p => p.BuildSteps)
                                                    .ThenInclude(s => s.BuildPluginParameters)
                                                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new NotFoundException("Project", projectId);

            var executiveBuild = new ExecutiveBuildDTO();

            executiveBuild.ProjectId = project.Id;
            executiveBuild.RepositoryUrl = project.Repository;
            executiveBuild.BuildSteps = project.BuildSteps
                .Select(buildstep => Mapper.Map<ExecutiveBuildStepDTO>(buildstep))
                .OrderBy(buildstep => buildstep.Index);

            return executiveBuild;
        }
    

        public async Task ChangeFavoriteStateAsync(int projectId)
        {
            var project = await Context.Set<Project>().AsNoTracking().SingleAsync(entity => entity.Id == projectId);
            project.IsFavorite = !project.IsFavorite;

            Context.Entry(project).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        public async Task<ProjectDTO> CopyProject(ProjectDTO dto)
        {
            var newProject = new ProjectDTO
            {
                Description = dto.Description,
                Name = dto.Name,
                OwnerId = dto.OwnerId,
                IsPublic = dto.IsPublic,
                IsFavorite = dto.IsFavorite,
                Repository = dto.Repository,
                CredentialsId = dto.CredentialsId,
                IsAutoCancelBranchBuilds = dto.IsAutoCancelBranchBuilds,
                IsCleanUpBeforeBuild = dto.IsCleanUpBeforeBuild,
                IsAutoCancelPullRequestBuilds = dto.IsAutoCancelPullRequestBuilds,
                CancelAfter = dto.CancelAfter,
            };

            var proj = Mapper.Map<Project>(newProject);
            var createdProject = (await Context.AddAsync(proj)).Entity;
            Context.SaveChanges();
            int id = createdProject.Id;
            dto.BuildSteps.Select(x => _buildStepService.Create(new NewBuildStepDTO
            {
                ProjectId = id,
                BuildStepName = x.BuildStepName,
                BuildPluginId = x.BuildPluginId,
                BuildPluginParameters = x.BuildPluginParameters,
                LoggingVerbosity = x.LoggingVerbosity
            }));
            var project = await Context.Projects
                                                .Include(p => p.BuildSteps)
                                                .Include(p => p.Owner)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            return Mapper.Map<ProjectDTO>(project);
        }
    }
}