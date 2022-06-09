import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { WorkNumberService } from '../../shared/work-number.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IClientWorkNumberPolicy, ClientService } from '@ds/core/clients/shared';
import { forkJoin } from 'rxjs';
import { IClientData } from '@ajs/onboarding/shared/models';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { AssetHelperService } from '@ds/core/ui/ui-helper';

@Component({
  selector: 'ds-work-number',
  templateUrl: './work-number-modal.component.html',
  styleUrls: ['./work-number-modal.component.scss']
})
export class WorkNumberModalComponent implements OnInit {
  user: UserInfo;
  isLoading: Boolean = true;
  isWorkNumberLoading: Boolean = true;
  workNumberPolicy: IClientWorkNumberPolicy;
  clientData: IClientData;
  isAccepted: number;
  isYes: boolean;

  constructor(
    private dialogRef: MatDialogRef<WorkNumberModalComponent>,
    private workNumberService: WorkNumberService,
    private accountService: AccountService,
    private clientService: ClientService,
    private msg:  DsMsgService,
    private assets: AssetHelperService
  ) {   }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe(u => {
      this.user = u;

      forkJoin (
        this.workNumberService.getWorkNumberInfo(u.clientId),
        this.clientService.getClientById(u.clientId)
      )
      .subscribe((results) => {
        this.workNumberPolicy = results[0];
        this.clientData = results[1];
        this.isAccepted = ( this.workNumberPolicy ) ? (this.workNumberPolicy.isAccepted ? 1 : 0 ) : 1;
        this.isWorkNumberLoading = false;
      });
      this.isLoading = false;
    });
  }

  userSave() {

    if (this.isAccepted === 1) {
      this.isYes = true;
    } else {
      this.isYes = false;
    }

    const workNumberParams: IClientWorkNumberPolicy = {
      clientId : this.user.clientId,
      clientWorkNumberPolicyId : this.workNumberPolicy ? this.workNumberPolicy.clientWorkNumberPolicyId : 0,
      acceptedBy : this.user.userId,
      isAccepted : this.isYes,
      modified : new Date(),
      user: null
    };

    this.workNumberService.updateAllowEquifax(workNumberParams)
    .subscribe((x) => {
      if (this.isYes) {
        this.msg.setTemporarySuccessMessage('Thank you for integrating with Equifax. You can choose to opt out at any time.', 5000);
      } else {
        this.msg
        .setTemporarySuccessMessage('You have chosen to not integrate with Equifax at this time. You can choose to do so at any time.'
        , 5000);
      }
    });
  }

  getAssetsPath(path: string): string {
    return this.assets.resolveAsset(path);
  }

  saveClose() {
    this.userSave();
    this.dialogRef.close(null);
  }

  close() {
    this.dialogRef.close(null);
  }

}
