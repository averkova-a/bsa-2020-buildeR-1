﻿using System.Threading.Tasks;
using buildeR.BLL.Interfaces;
using buildeR.Common.DTO.Webhooks.Github.PayloadDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buildeR.API.Controllers
{
    [AllowAnonymous]
    [Route("{controller}", Name = "Webhooks")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly IWebhooksHandler _handler;
        public WebhooksController(IWebhooksHandler handler)
        {
            _handler = handler;
        }

        [HttpPost()]
        public void WebhookCallback()
        {
            //this method is used for generating callback links for repo providers
            //please, don't touch it :3
        }

        [HttpPost("{projectId}/github/push")]
        public async Task GithubWebhookPushCallback(int projectId, [FromBody]PushGithubPayloadDTO payload)
        {
            await _handler.HandleGithubPushEvent(projectId, payload);
        }

        [HttpPost("{projectId}/github/pull_request")]
        public async Task GithubWebhookPullRequestCallback(int projectId, [FromBody]PullRequestGithubPayloadDTO payload)
        {
            await _handler.HandleGithubPullRequestEvent(projectId, payload);
        }
    }
}
