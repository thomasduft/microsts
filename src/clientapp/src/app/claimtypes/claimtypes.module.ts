import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../shared';
import { SharedModule } from '../shared/shared.module';
import { FormdefModule } from '../shared/formdef/formdef.module';

import { ClaimTypesService } from './services';
import { ClaimtypeDetailComponent } from './components/detail/claimtype-detail.component';
import { ClaimtypeListComponent } from './components/list/claimtype-list.component';
import { ClaimtypeDashboardComponent } from './components/dashboard/claimtype-dashboard.component';
import { FormdefRegistry } from '../shared/formdef';
import { ClaimtypeDetailSlot } from './models';

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
    SharedModule,
    FormdefModule
  ],
  declarations: [
    ClaimtypeListComponent,
    ClaimtypeDetailComponent,
    ClaimtypeDashboardComponent
  ],
  providers: [
    ClaimTypesService
  ]
})
export class ClaimtypesModule {
  public constructor(
    private slotRegistry: FormdefRegistry
  ) {
    this.slotRegistry.register(new ClaimtypeDetailSlot());
  }
}
