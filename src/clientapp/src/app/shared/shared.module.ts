import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ForbiddenComponent } from './components/forbidden.component';
import { PageNotFoundComponent } from './components/page-not-found.component';

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    ForbiddenComponent,
    PageNotFoundComponent
  ],
  exports: [
    ForbiddenComponent,
    PageNotFoundComponent
  ]
})
export class SharedModule { }
