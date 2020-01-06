import { Subject, BehaviorSubject, Observable } from 'rxjs';

import { Injectable } from '@angular/core';

import { MessageBus } from './../../../shared/services/messageBus.service';
import { IMessageSubscriber } from './../../../shared/services/models';
import { StatusMessage } from './models';

@Injectable()
export class StatusBarService implements IMessageSubscriber<StatusMessage> {
  private status$: Subject<StatusMessage> = new BehaviorSubject<StatusMessage>(null);

  public get status(): Observable<StatusMessage> {
    return this.status$.asObservable();
  }

  public constructor(
    private messageBus: MessageBus
  ) {
    this.messageBus.subsribe(this);
  }

  public onMessage(message: StatusMessage): void {
    this.setStatus(message);
  }

  public getType(): string {
    return StatusMessage.KEY;
  }

  private setStatus(message: StatusMessage): void {
    this.status$.next(message);
  }
}
