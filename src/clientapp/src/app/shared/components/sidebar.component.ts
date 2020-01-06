import { Component, HostBinding, Output, EventEmitter } from '@angular/core';

import { UserService } from '../services';

@Component({
  selector: 'tw-sidebar',
  template: `
  <div class="sidebar__header" (click)="toggle()">
    <div *ngIf="!collapsed">MicroWF</div>
    <button type="button">
      <span *ngIf="collapsed" name="arrow-right"><b>></b></span>
      <span *ngIf="!collapsed" name="arrow-left"><b><</b></span>
    </button>
  </div>
  <div class="sidebar__content">
    <ul>
      <li routerLinkActive="active" class="menu__item">
        <a routerLink="/home">
          <!-- <tw-icon name="home"></tw-icon> -->
          {{ userName }}
        </a>
      </li>
    </ul>
  </div>
  <div class="sidebar__footer">
    <ul>
      <li routerLinkActive="active" *ngIf="!isAuthenticated">
        <a href="javascript:void(0)" (click)="login()">
          Login
        </a>
      </li>
      <li routerLinkActive="active" *ngIf="isAuthenticated">
        <a href="javascript:void(0)" (click)="logout()">
          Logout
        </a>
      </li>
    </ul>
  </div>
  `
})
export class SidebarComponent {
  public collapsed = false;

  public get isAuthenticated(): boolean {
    return this.user.isAuthenticated;
  }

  public get userName(): string {
    return this.user.userName;
  }

  @HostBinding('class')
  public classlist = this.getClassList();

  @Output()
  public loginClick: EventEmitter<string> = new EventEmitter<string>();

  public constructor(
    private user: UserService
  ) { }

  public toggle(): void {
    this.collapsed = !this.collapsed;
    this.classlist = this.getClassList();
  }

  public login(): void {
    this.loginClick.next('login');
  }

  public logout(): void {
    this.loginClick.next('logout');
  }

  private getClassList(): string {
    if (this.collapsed) {
      return 'sidebar sidebar--collapsed';
    }

    return 'sidebar';
  }
}
