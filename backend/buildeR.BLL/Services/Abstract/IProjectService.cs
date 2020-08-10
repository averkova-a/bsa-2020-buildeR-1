﻿using buildeR.Common.DTO.Project;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace buildeR.BLL.Services.Abstract
{
    public interface IProjectService : ICrudService<ProjectDTO, NewProjectDTO, int>
    {
        Task<IEnumerable<ProjectInfoDTO>> GetProjectsByUser(int userId);
        Task<ProjectDTO> GetProjectByUserId(int userId, int projectId);
        Task<ProjectDTO> CreateProject(NewProjectDTO dto);
        Task UpdateProject(ProjectDTO dto, int userId);

        Task DeleteProject(int id);
    }
}
