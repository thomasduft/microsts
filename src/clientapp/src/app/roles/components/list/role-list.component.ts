import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';

import { AutoUnsubscribe } from '../../../shared';

import { Role } from '../../models';
import { RoleService } from '../../services/role.service';

@AutoUnsubscribe
@Component({
  selector: 'tw-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.less'],
  providers: [
    RoleService
  ]
})
export class RoleListComponent implements OnInit {
  private roles$: Subscription;

  public roles: Array<Role> = [];

  public constructor(
    private service: RoleService
  ) { }

  public ngOnInit(): void {
    this.loadData();
  }

  public reload(): void {
    this.ngOnInit();
  }

  private loadData(): void {
    this.loadRoles();
  }

  private loadRoles(): void {
    this.roles$ = this.service.roles()
      .subscribe((response: Array<Role>) => {
        this.roles = response;
      });
  }
}
