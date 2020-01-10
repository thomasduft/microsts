import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { AutoUnsubscribe, MessageBus, StatusMessage, StatusLevel } from '../../../shared';

import { ClaimtypeDetailSlot, ClaimType } from '../../models';
import { ClaimTypesService } from '../../services';

@AutoUnsubscribe
@Component({
  selector: 'tw-claimtype-detail',
  templateUrl: './claimtype-detail.component.html',
  styleUrls: ['./claimtype-detail.component.less'],
  providers: [
    ClaimTypesService
  ]
})
export class ClaimtypeDetailComponent implements OnInit {
  private routeParams$: Subscription;
  private claimtype$: Subscription;

  public key = ClaimtypeDetailSlot.KEY;
  public viewModel: ClaimType;

  public isNew = false;

  public constructor(
    private route: ActivatedRoute,
    private service: ClaimTypesService,
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

  public submitted(viewModel: ClaimType): void {
    if (this.isNew) {
      this.claimtype$ = this.service.create(viewModel)
        .subscribe((id: string) => {
          if (id) {
            console.log('created', id);
            this.create();
          }
        });
    } else {
      this.claimtype$ = this.service.update(viewModel)
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

  public deleted(viewModel: ClaimType): void {
    if (this.isNew) {
      return;
    }

    // TODO: confirm???

    this.claimtype$ = this.service.delete(viewModel.id)
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
    this.claimtype$ = this.service.claimtype(id)
      .subscribe((result: ClaimType) => {
        this.key = ClaimtypeDetailSlot.KEY;
        this.viewModel = result;
      });
  }

  private create(): void {
    this.isNew = true;
    this.viewModel = {
      id: 'new',
      name: undefined,
      description: undefined
    };
  }
}
