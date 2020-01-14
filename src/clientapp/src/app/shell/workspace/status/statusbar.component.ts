import { Component, HostBinding, OnInit } from '@angular/core';

import { StatusMessage, StatusLevel } from '../../../shared/services';
import { StatusBarService } from './statusbar.service';

@Component({
  selector: 'tw-statusbar',
  providers: [
    StatusBarService
  ],
  template: `
  <div *ngIf="displayStatusBar()" [class]="statusClass">
    <button type="button" class="close"(click)="close()">
      <span>&times;</span>
    </button>
    <button *ngIf="hasAction"
            type="button"
            class="close"
            (click)="action()">
      <span aria-hidden="true">action</span>
    </button>
    <strong *ngIf="status.title">{{ status.title }}:</strong>{{ status.message }}
  </div>`
})
export class StatusBarComponent implements OnInit {
  private message: StatusMessage;

  @HostBinding('class') workspaceNotification = 'workspace__status';

  public hasAction = false;

  public get status(): StatusMessage {
    return this.message;
  }

  public get statusClass(): string {
    let s = 'success';

    switch (this.message.level) {
      case StatusLevel.Info:
        s = 'info';
        break;
      case StatusLevel.Warning:
        s = 'warning';
        break;
      case StatusLevel.Danger:
        s = 'danger';
        break;
    }

    return `alert alert-${s}`;
  }

  public constructor(
    private service: StatusBarService
  ) { }

  public ngOnInit(): void {
    this.service.status
      .subscribe((status: StatusMessage) => {
        if (!status) {
          // initial StatusMessage will be null!
          return;
        }

        this.message = status;
        this.hasAction = this.message.hasAction;
        this.fadeOut();
      });
  }

  public displayStatusBar(): boolean {
    return this.message && !this.message.viewed;
  }

  public close(): void {
    this.message.viewed = true;
  }

  public action(): void {
    this.message.action();
  }

  private fadeOut(): void {
    if (!this.message
      || this.message.level !== StatusLevel.Success) {
      return;
    }

    setTimeout(() => {
      this.message.viewed = true;
    }, 3000);
  }
}
