import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { IconModule } from './icon/icon.module';
import { ListModule } from './list/list.module';
import { TabModule } from './tab/tab.module';

import { ForbiddenComponent } from './components/forbidden.component';
import { PageNotFoundComponent } from './components/page-not-found.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    IconModule,
    ListModule,
    TabModule
  ],
  declarations: [
    ForbiddenComponent,
    PageNotFoundComponent
  ],
  exports: [
    ForbiddenComponent,
    PageNotFoundComponent,
    IconModule,
    ListModule,
    TabModule
  ]
})
export class SharedModule { }
