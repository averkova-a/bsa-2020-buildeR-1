﻿using buildeR.Common.DTO.BuildStep;

using System.Collections.Generic;

namespace buildeR.Common.DTO.BuildHistory
{
    public sealed class ExecutiveBuildDTO
    {
        public int ProjectId { get; set; }
        public string RepositoryUrl { get; set; }
        public IEnumerable<ExecutiveBuildStepDTO> BuildSteps { get; set; }
    }
}