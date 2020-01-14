import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { AutoUnsubscribe, MessageBus, StatusMessage, StatusLevel } from '../../../shared';
import { RefreshMessage } from '../../../core';

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
    private router: Router,
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
            this.back();
          }
        });
    } else {
      this.claimtype$ = this.service.update(viewModel)
        .subscribe(() => {
          this.back();
        });
    }

    this.changesSaved();
  }

  public deleted(viewModel: ClaimType): void {
    if (this.isNew) {
      return;
    }

    // TODO: confirm???

    this.claimtype$ = this.service.delete(viewModel.id)
      .subscribe((id: string) => {
        this.back();
      });

    this.changesSaved();
  }

  public back(): void {
    this.router.navigate(['claimtypes']);
  }

  private init(id?: string): void {
    if (id !== 'new') {
      this.load(id.toString());
    } else {
      this.create();
    }
  }

  private load(id: string): void {
    this.isNew = false;
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

  private changesSaved(): void {
    this.messageBus.publish(
      new StatusMessage(
        undefined,
        'Your changes have been saved...',
        StatusLevel.Success
      ));

    this.messageBus.publish(new RefreshMessage('claimtype'));
  }
}
