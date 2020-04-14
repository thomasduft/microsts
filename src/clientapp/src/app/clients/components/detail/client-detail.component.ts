import { Subscription } from 'rxjs';

import { Component, OnInit, ElementRef } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import {
  AutoUnsubscribe,
  MessageBus,
  StatusMessage,
  StatusLevel,
  Popover,
  DeleteConfirmationComponent,
  PopoverCloseEvent,
  DeleteConfirmation
} from '../../../shared';
import { FormdefRegistry } from '../../../shared/formdef';
import { RefreshMessage } from '../../../core';

import { ClientDetailSlot, Client } from '../../models';
import { ClientService } from '../../services';

@AutoUnsubscribe
@Component({
  selector: 'tw-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.less'],
  providers: [
    ClientService
  ]
})
export class ClientDetailComponent implements OnInit {
  private routeParams$: Subscription;
  private client$: Subscription;

  public key = ClientDetailSlot.KEY;
  public viewModel: Client;

  public isNew = false;

  public constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: ClientService,
    private slotRegistry: FormdefRegistry,
    private popup: Popover,
    private element: ElementRef,
    private messageBus: MessageBus
  ) { }

  public ngOnInit(): void {
    this.routeParams$ = this.route.params
      .subscribe((params: Params) => {
        if (params.clientId) {
          this.init(params.clientId);
        }
      });
  }

  public submitted(viewModel: Client): void {
    if (this.isNew) {
      this.client$ = this.service.create(viewModel)
        .subscribe(() => {
          this.changesSaved();
          this.back();
        });
    } else {
      this.client$ = this.service.update(viewModel)
        .subscribe(() => {
          this.changesSaved();
          this.back();
        });
    }
  }

  public deleted(viewModel: Client): void {
    if (this.isNew) {
      return;
    }

    const origin = this.element.nativeElement;

    const popoverRef = this.popup
      .open<DeleteConfirmation>({
        content: DeleteConfirmationComponent,
        origin,
        hasBackdrop: false,
        data: {
          confirm: false,
          itemText: viewModel.clientId
        }
      });

    popoverRef.afterClosed$
      .subscribe((res: PopoverCloseEvent<DeleteConfirmation>) => {
        if (res.data.confirm) {
          this.client$ = this.service.delete(viewModel.clientId)
            .subscribe((id: string) => {
              this.changesSaved();
              this.back();
            });
        }
      });
  }

  public back(): void {
    this.router.navigate(['clients']);
  }

  private init(clientId?: string): void {
    if (clientId !== 'new') {
      this.load(clientId);
    } else {
      this.create();
    }
  }

  private load(clientId: string): void {
    this.isNew = false;
    this.client$ = this.service.client(clientId)
      .subscribe((result: Client) => {
        this.slotRegistry.register(new ClientDetailSlot(
          result.allowedGrantTypes,
          result.redirectUris,
          result.postLogoutRedirectUris,
          result.allowedCorsOrigins,
          result.allowedScopes
        ));

        this.key = ClientDetailSlot.KEY;
        this.viewModel = result;
      });
  }

  private create(): void {
    this.isNew = true;
    this.viewModel = {
      id: 0,
      enabled: true,
      clientId: 'new',
      clientName: undefined,
      requireClientSecret: false,
      clientSecret: undefined,
      requirePkce: false,
      requireConsent: false,
      allowAccessTokensViaBrowser: false,
      allowedGrantTypes: [],
      redirectUris: [],
      postLogoutRedirectUris: [],
      allowedCorsOrigins: [],
      allowedScopes: []
    };

    this.slotRegistry.register(new ClientDetailSlot(
      this.viewModel.allowedGrantTypes,
      this.viewModel.redirectUris,
      this.viewModel.postLogoutRedirectUris,
      this.viewModel.allowedCorsOrigins,
      this.viewModel.allowedScopes
    ));
  }

  private changesSaved(): void {
    this.messageBus.publish(
      new StatusMessage(
        undefined,
        'Your changes have been saved...',
        StatusLevel.Success
      ));

    this.messageBus.publish(new RefreshMessage('client'));
  }
}
