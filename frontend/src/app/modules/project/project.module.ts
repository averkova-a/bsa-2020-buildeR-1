import { NgModule } from '@angular/core';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectCreateComponent } from './project-create/project-create.component';
import { ProjectSettingsComponent } from './project-settings/project-settings.component';
import { SharedModule } from '@shared/shared.module';
import { ProjectComponent } from './project.component';
import { LoggingTerminalComponent } from './logging-terminal/logging-terminal.component';
import { ProjectDetailsComponent } from './project-details/project-details.component';

@NgModule({
  declarations: [ProjectComponent, ProjectCreateComponent, ProjectSettingsComponent, LoggingTerminalComponent, ProjectDetailsComponent],
  imports: [ProjectRoutingModule, SharedModule],
})
export class ProjectModule { }