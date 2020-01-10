import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../shared';
import { SharedModule } from '../shared/shared.module';
import { FormdefModule } from '../shared/formdef/formdef.module';

import { AccountService } from './services';
import { UserDashboardComponent } from './components/dashboard/user-dashboard.component';
import { UserListComponent } from './components/list/user-list.component';
import { UserDetailComponent } from './components/detail/user-detail.component';

const ROUTES: Routes = [
  {
    path: 'users',
    component: UserDashboardComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: ':id',
        component: UserDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ROUTES),
    SharedModule,
    FormdefModule
  ],
  declarations: [
    UserListComponent,
    UserDetailComponent,
    UserDashboardComponent
  ],
  providers: [
    AccountService
  ]
})
export class UsersModule {

}
