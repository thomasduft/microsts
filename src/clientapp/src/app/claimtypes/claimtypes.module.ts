import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../shared';
import { SharedModule } from '../shared/shared.module';

import { ClaimtypeDetailComponent } from './components/detail/claimtype-detail.component';
import { ClaimtypeListComponent } from './components/list/claimtype-list.component';
import { ClaimtypeDashboardComponent } from './components/dashboard/claimtype-dashboard.component';

const ROUTES: Routes = [
  {
    path: 'claimtypes',
    component: ClaimtypeDashboardComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: ':id',
        component: ClaimtypeDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ROUTES),
    SharedModule
  ],
  declarations: [
    ClaimtypeListComponent,
    ClaimtypeDetailComponent,
    ClaimtypeDashboardComponent
  ]
})
export class ClaimtypesModule { }
