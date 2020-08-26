﻿using buildeR.BLL.Interfaces;
using buildeR.BLL.Services.Abstract;
using buildeR.Common.DTO.Synchronization;
using buildeR.Common.DTO.Synchronization.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buildeR.BLL.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly IGithubClient _githubClient;
        private readonly IProjectService _projectService;
        private readonly ISynchronizationHelper _synchronizationHelper;
        public SynchronizationService(IGithubClient githubClient,
                                      IProjectService projectService,
                                      ISynchronizationHelper synchronizationHelper)
        {
            _githubClient = githubClient;
            _projectService = projectService;
            _synchronizationHelper = synchronizationHelper;
        }
        public async Task<IEnumerable<Branch>> GetRepositoryBranches(int projectId)
        {
            var repository = await _projectService.GetRepository(projectId);

            IEnumerable<GithubBranch> branches = null;

            if (repository.Private)
                branches = await _githubClient.GetPrivateRepositoryBranches(repository.Name, repository.Username, repository.Password);
            else
                branches = await _githubClient.GetPublicRepositoryBranches(repository.Name, repository.Owner);

            return branches.Select(b => new Branch { Name = b.Name });
        }
        public async Task<IEnumerable<Repository>> GetUserRepositories(string username, string password)
        {
            var repositories = await _githubClient.GetUserRepositories(username, password);

            return repositories.Select(r => new Repository { Name = r.Name, Owner = r.Owner.Login, Private = r.Private });
        }
        public async Task<bool> CheckIfRepositoryAccessable(string repoUrl, string username = null, string password = null)
        {
            var repoName = _synchronizationHelper.GetRepositoryNameFromUrl(repoUrl);
            var repoOwner = _synchronizationHelper.GetRepositoryOwnerFromUrl(repoUrl);

            return await _githubClient.CheckIfRepositoryAccessable(repoName, repoOwner, username, password);
        }
        public async Task RegisterWebhook(int projectId, string callback)
        {
            callback += $"/{projectId}/github";

            var repository = await _projectService.GetRepository(projectId);
            await _githubClient.CreateWebhook(repository.Name, callback, repository.Username, repository.Password);
        }

        //public async Task<IEnumerable<Branch>> GetRepositoryBranches(int projectId, string accessToken)
        //{
        //    var repository = await _projectService.GetRepository(projectId);
        //   IEnumerable<GithubBranch> branches = null;

        //    if (repository.Owner != null)
        //        branches = await _githubClient.GetRepositoryBranches(repository.Owner, repository.Name, accessToken);
        //    else
        //        branches = await _githubClient.GetRepositoryBranches(repository.Name, accessToken);

        //    return branches.Select(b => new Branch { Name = b.Name });
        //}

        //public async Task<IEnumerable<Repository>> GetUserRepositories(string accessToken)
        //{
        //    var repos = await _githubClient.GetUserRepositories(accessToken);
        //    return repos.Select(r => new Repository { Id = r.Id, Name = r.Name, Private = r.Private });
        //}
        //public async Task<bool> CheckIfRepositoryAccessable(string repoUrl)
        //{
        //    string owner = "";
        //    string repo = "";

        //    try
        //    {
        //        owner = _synchronizationHelper.GetRepositoryOwnerFromUrl(repoUrl);
        //        repo = _synchronizationHelper.GetRepositoryNameFromUrl(repoUrl);
        //    }
        //    catch(Exception)
        //    {
        //        return false;
        //    }

        //    return await _githubClient.CheckIfRepositoryAccessable(owner, repo);
        //}

        //public async Task RegisterWebhook(int projectId, string callback, string accessToken)
        //{
        //    callback += $"/{projectId}/github";

        //    var repository = await _projectService.GetRepository(projectId);
        //    await _githubClient.CreateWebhook(repository.Name, callback, accessToken);
        //}
    }
}
