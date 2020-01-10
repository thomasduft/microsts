import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../shared';
import { SharedModule } from '../shared/shared.module';
import { FormdefModule } from '../shared/formdef/formdef.module';
import { FormdefRegistry } from '../shared/formdef';

import { AccountService } from './services';

const ROUTES: Routes = [
  // {
  //   path: 'users',
  //   component: UserDashboardComponent,
  //   canActivate: [AuthGuard],
  //   children: [
  //     {
  //       path: ':id',
  //       component: UserDetailComponent
  //     }
  //   ]
  // }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ROUTES),
    SharedModule,
    FormdefModule
  ],
  declarations: [],
  providers: [
    AccountService
  ]
})
export class UsersModule {
  public constructor(
    private slotRegistry: FormdefRegistry
  ) {
    // this.slotRegistry.register(new UserDetailSlot());
  }
}
