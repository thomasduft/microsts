import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../shared';

import { ClaimtypeDetailComponent } from './components/detail/claimtype-detail.component';
import { ClaimtypeListComponent } from './components/list/claimtype-list.component';
import { ClaimtypeDashboardComponent } from './components/dashboard/claimtype-dashboard.component';

const ROUTES: Routes = [
  {
    path: 'claimtypes',
    component: ClaimtypeDashboardComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ROUTES)
  ],
  declarations: [
    ClaimtypeListComponent,
    ClaimtypeDetailComponent,
    ClaimtypeDashboardComponent
  ]
})
export class ClaimtypesModule { }
