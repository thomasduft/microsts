import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExpanderModule } from './expander/expander.module';
import { TabModule } from './tab/tab.module';

import { ForbiddenComponent } from './components/forbidden.component';
import { PageNotFoundComponent } from './components/page-not-found.component';

@NgModule({
  imports: [
    CommonModule,
    ExpanderModule,
    TabModule
  ],
  declarations: [
    ForbiddenComponent,
    PageNotFoundComponent
  ],
  exports: [
    ExpanderModule,
    TabModule,
    ForbiddenComponent,
    PageNotFoundComponent
  ]
})
export class SharedModule { }
