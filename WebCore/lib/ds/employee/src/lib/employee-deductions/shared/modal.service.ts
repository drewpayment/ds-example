import { Injectable } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { DeductionsAddEditModalComponent } from '../deductions/deductions-add-edit-modal/deductions-add-edit-modal.component';
import { IDeductions, IDeductionsData, IDeductionsResult, Deductions, IClientDeductionInfo } from '../models/Deductions';
import { DirectDepositAddEditModalComponent } from '../direct-deposit/direct-deposit-add-edit-modal/direct-deposit-add-edit-modal.component';
import { EarningsAddEditModalComponent } from '../earnings/earnings-add-edit-modal/earnings-add-edit-modal.component';
import { UserInfo } from '@ds/core/shared';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  constructor(private dialog: MatDialog) { }

  openDeductionsModal(deductionsData: IDeductions, clientInfo: IClientDeductionInfo, userInfo: UserInfo, reminderDateTime: Date | null = null){
    let config = new MatDialogConfig<IDeductionsData>();
    let option = deductionsData == null ? 'add' : 'edit';

    config.data = {
        data: deductionsData,
        option: option,
        clientInfo: clientInfo,
        userInfo: userInfo,
        reminderDateTime: reminderDateTime
    };

    config.width = '500px';

    return this.dialog.open<DeductionsAddEditModalComponent, IDeductionsData, IDeductionsResult>(DeductionsAddEditModalComponent, config);
  }

  openDirectDepositModal(deductionsData: Deductions | null, clientInfo: IClientDeductionInfo, userInfo: UserInfo, reminderDateTime: Date | null = null, multipleHundreds: boolean, fromHSA: boolean = false, fromHSAEdit: boolean = false){
    let config = new MatDialogConfig<IDeductionsData>();
    let option = ""

    if( deductionsData == null ){
        option = 'add';
    }
    else if( deductionsData != null && fromHSA == false){
        option = 'edit';
    }
    else if( deductionsData != null && fromHSA == true && fromHSAEdit == true){
        option = "editHSA";
    }
    else if( deductionsData != null && fromHSA == true && fromHSAEdit == false){
        option = "addHSA";
    }

    config.data = {
        data: deductionsData,
        option: option,
        clientInfo: clientInfo,
        userInfo: userInfo,
        reminderDateTime: reminderDateTime,
        fromHSA: fromHSA,
        multipleHundreds: multipleHundreds
    };

    config.width = '500px';

    return this.dialog.open<DirectDepositAddEditModalComponent, IDeductionsData, IDeductionsResult>(DirectDepositAddEditModalComponent, config);
  }

  openEarningsModal(deductionsData: IDeductions, usedEarningsArray: number[], clientInfo: IClientDeductionInfo, userInfo: UserInfo, reminderDateTime: Date | null = null){
    let config = new MatDialogConfig<IDeductionsData>();
    let option = deductionsData == null ? 'add' : 'edit';

    config.data = {
        data: deductionsData,
        option: option,
        clientInfo: clientInfo,
        userInfo: userInfo,
        reminderDateTime: reminderDateTime,
        usedEarningsIds: usedEarningsArray
    };

    config.width = '500px';

    return this.dialog.open<EarningsAddEditModalComponent, IDeductionsData, IDeductionsResult>(EarningsAddEditModalComponent, config);
  }
}
