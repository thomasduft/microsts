import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ExpanderModule } from './expander/expander.module';
import { IconModule } from './icon/icon.module';
import { ListModule } from './list/list.module';
import { PaneModule } from './pane/pane.module';
import { TabModule } from './tab/tab.module';

import { ForbiddenComponent } from './components/forbidden.component';
import { PageNotFoundComponent } from './components/page-not-found.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ExpanderModule,
    IconModule,
    ListModule,
    PaneModule,
    TabModule
  ],
  declarations: [
    ForbiddenComponent,
    PageNotFoundComponent
  ],
  exports: [
    ForbiddenComponent,
    PageNotFoundComponent,
    ExpanderModule,
    IconModule,
    ListModule,
    PaneModule,
    TabModule
  ]
})
export class SharedModule { }
