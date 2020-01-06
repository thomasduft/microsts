import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ForbiddenComponent } from './components/forbidden.component';
import { PageNotFoundComponent } from './components/page-not-found.component';
import { SidebarComponent } from './components/sidebar.component';
import { WorkspaceComponent } from './components/workspace.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [
    ForbiddenComponent,
    PageNotFoundComponent,
    SidebarComponent,
    WorkspaceComponent
  ],
  exports: [
    ForbiddenComponent,
    PageNotFoundComponent,
    SidebarComponent,
    WorkspaceComponent
  ]
})
export class SharedModule { }
