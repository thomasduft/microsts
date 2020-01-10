import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { AutoUnsubscribe, MessageBus, StatusMessage, StatusLevel } from '../../../shared';

import { RoleDetailSlot, Role } from '../../models';
import { RoleService } from '../../services';

@AutoUnsubscribe
@Component({
  selector: 'tw-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.less'],
  providers: [
    RoleService
  ]
})
export class RoleDetailComponent implements OnInit {
  private routeParams$: Subscription;
  private role$: Subscription;

  public key = RoleDetailSlot.KEY;
  public viewModel: Role;

  public isNew = false;

  public constructor(
    private route: ActivatedRoute,
    private service: RoleService,
    private messageBus: MessageBus
  ) { }

  public ngOnInit(): void {
    this.routeParams$ = this.route.params
      .subscribe((params: Params) => {
        if (params.id) {
          this.init(params.id);
        }
      });
  }

  public submitted(viewModel: Role): void {
    if (this.isNew) {
      this.role$ = this.service.create(viewModel)
        .subscribe((id: string) => {
          if (id) {
            console.log('created', id);
            this.create();
          }
        });
    } else {
      this.role$ = this.service.update(viewModel)
        .subscribe(() => {
          console.log('updated', viewModel);
        });
    }

    this.messageBus.publish(
      new StatusMessage(
        undefined,
        'Your changes have been saved...',
        StatusLevel.Success
      ));
  }

  public deleted(viewModel: Role): void {
    if (this.isNew) {
      return;
    }

    // TODO: confirm???

    this.role$ = this.service.delete(viewModel.id)
      .subscribe((id: string) => {
        console.log('deleted', viewModel.id);
        this.create();
      });
  }

  public cancel(): void {
    console.log('cancel...');
  }

  private init(id?: string): void {
    if (id !== 'new') {
      this.load(id.toString());
    } else {
      this.create();
    }
  }

  private load(id: string): void {
    this.role$ = this.service.claimtype(id)
      .subscribe((result: Role) => {
        this.key = RoleDetailSlot.KEY;
        this.viewModel = result;
      });
  }

  private create(): void {
    this.isNew = true;
    this.viewModel = {
      id: 'new',
      name: undefined
    };
  }
}
