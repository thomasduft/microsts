import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';

import { AutoUnsubscribe } from '../../../shared';

import { User } from '../../models';
import { AccountService } from '../../services/account.service';

@AutoUnsubscribe
@Component({
  selector: 'tw-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.less'],
  providers: [
    AccountService
  ]
})
export class UserListComponent implements OnInit {
  private users$: Subscription;

  public users: Array<User> = [];

  public constructor(
    private service: AccountService
  ) { }

  public ngOnInit(): void {
    this.loadData();
  }

  public reload(): void {
    this.ngOnInit();
  }

  private loadData(): void {
    this.loadUsers();
  }

  private loadUsers(): void {
    this.users$ = this.service.users()
      .subscribe((response: Array<User>) => {
        this.users = response;
      });
  }
}
