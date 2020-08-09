﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using buildeR.BLL.Exceptions;
using buildeR.BLL.Services.Abstract;
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
        public ProjectService(BuilderContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override Task<ProjectDTO> GetAsync(int id, bool isNoTracking = false)
        {
            return base.GetAsync(id, isNoTracking);
        }

        public async Task<IEnumerable<ProjectInfoDTO>> GetProjectsByUser(int userId)
        {
            return await Context.Projects//TODO do need to check user existence?
                .AsNoTracking()
                .Include(project => project.Owner)
                .Include(project => project.BuildHistories)
                .Where(project => project.OwnerId.Equals(userId))
                .ProjectTo<ProjectInfoDTO>(Mapper.ConfigurationProvider)
                .ToArrayAsync();
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
    }
}