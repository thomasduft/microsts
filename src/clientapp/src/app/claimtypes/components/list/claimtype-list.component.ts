import { Subscription } from 'rxjs';

import { Component, OnInit } from '@angular/core';

import { AutoUnsubscribe } from '../../../shared';

import { ClaimType } from '../../models';
import { ClaimTypesService } from '../../services/claimtypes.service';

@AutoUnsubscribe
@Component({
  selector: 'tw-claimtype-list',
  templateUrl: './claimtype-list.component.html',
  styleUrls: ['./claimtype-list.component.less'],
  providers: [
    ClaimTypesService
  ]
})
export class ClaimtypeListComponent implements OnInit {
  private claimtypes$: Subscription;

  public claimtypes: Array<ClaimType> = [];

  public constructor(
    private service: ClaimTypesService
  ) { }

  public ngOnInit(): void {
    this.loadData();
  }

  public reload(): void {
    this.ngOnInit();
  }

  private loadData(): void {
    this.loadClaimtypes();
  }

  private loadClaimtypes(): void {
    this.claimtypes$ = this.service.claimtypes()
      .subscribe((response: Array<ClaimType>) => {
        this.claimtypes = response;
      });
  }
}
