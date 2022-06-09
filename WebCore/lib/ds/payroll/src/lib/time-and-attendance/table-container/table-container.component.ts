import {
  Component,
  OnInit,
  Input,
  ChangeDetectionStrategy,
  EventEmitter,
  Output,
  OnDestroy,
  ChangeDetectorRef,
  NgZone,
  ViewChildren,
  ElementRef,
  QueryList,
  AfterViewChecked,
} from "@angular/core";
import { DataRow } from "../shared/DataRow.model";
import { Maybe } from "@ds/core/shared/Maybe";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { tap, filter, startWith, map, takeUntil } from "rxjs/operators";
import { Observable, Subscription, merge, Subject } from "rxjs";
import { GetClockClientNoteListResultDto } from "../shared/InitData.model";
import { GetTimeApprovalTableDto } from "../shared/GetTimeApprovalTableDto.model";
import { AccountService, ACTION_TYPES } from "@ds/core/account.service";
import {
  coerceBooleanProperty,
  coerceNumberProperty,
} from "@angular/cdk/coercion";
import { DsPopupService } from "@ajs/ui/popup/ds-popup.service";
import { forkJoin } from "rxjs";
import { ClockService } from "@ds/core/employee-services/clock.service";
import * as moment from "moment";
import { DomSanitizer } from "@angular/platform-browser";
import { Moment } from "moment";
import { PunchMapModalComponent } from "../punch-map-modal/punch-map-modal.component";
import { MatDialogRef, MatDialog } from "@angular/material/dialog";
import { EmployeePunchData } from "../shared/employee_punch_map_info.model";
import { AppConfig } from "@ds/core/app-config/app-config";
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Component({
  selector: "ds-table-container",
  templateUrl: "./table-container.component.html",
  styleUrls: ["./table-container.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableContainerComponent
  implements OnInit, OnDestroy, AfterViewChecked
{
  readonly daysOfWeek = [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
  ];
  private unsubscriber = new Subject();
  private _tableData: GetTimeApprovalTableDto;
  approvedCheckboxes: FormGroup;
  ApproveAllCheckboxesControl: FormControl = new FormControl();
  changeListener$: Observable<any>;
  listenToForm: Subscription;
  listenToSelectInputs: Subscription;
  listOfPages = [];
  changedControls: boolean[] = [];
  readRate = false;
  private _isLoading = true;
  selectedRow = 0;
  selectedPunch: string = null;
  vbDate = "M/D/YYYY";
  employeePunchStartDate: Moment;
  employeePunchEndDate: Moment;

  get isLoading(): boolean {
    return this._isLoading;
  }
  set isLoading(value: boolean) {
    this._isLoading = coerceBooleanProperty(value);
    this.changeDetector.detectChanges();
  }

  @Input()
  public get tableData(): GetTimeApprovalTableDto {
    return this._tableData;
  }
  public set tableData(value: GetTimeApprovalTableDto) {
    this.ApproveAllCheckboxesControl.setValue(false);
    const safeVal = new Maybe(value);
    this.createForm(safeVal.map((x) => x.table));
    this.listOfPages = new Array(safeVal.map((x) => x.totalPages).valueOr(0));
    this._tableData = value;
    this._tableData.table.forEach((rowOfData) => {
      if (rowOfData && rowOfData.notes) {
        rowOfData.notes = this.removeBreaksFromStrings(rowOfData.notes);
      }
      if (rowOfData.inToolTipContent !== null) {
        rowOfData.inToolTipContent = this.removeBreaksFromStrings(
          rowOfData.inToolTipContent
        );
      }
      if (rowOfData.in2ToolTipContent !== null) {
        rowOfData.in2ToolTipContent = this.removeBreaksFromStrings(
          rowOfData.in2ToolTipContent
        );
      }
      if (rowOfData.outToolTipContent !== null) {
        rowOfData.outToolTipContent = this.removeBreaksFromStrings(
          rowOfData.outToolTipContent
        );
      }
      if (rowOfData.out2ToolTipContent !== null) {
        rowOfData.out2ToolTipContent = this.removeBreaksFromStrings(
          rowOfData.out2ToolTipContent
        );
      }
      if (rowOfData.lnkDateToolTipContent !== null) {
        rowOfData.lnkDateToolTipContent = this.removeBreaksFromStrings(
          rowOfData.lnkDateToolTipContent
        );
      }
    });
  }

  @Input() hasSearchedEmps: boolean;
  @Input() searchEmpsInFlight: boolean;
  @Input() noteOptions: GetClockClientNoteListResultDto[];
  @Output() formValue: EventEmitter<any> = new EventEmitter();
  @Output() popUpClosed: EventEmitter<any> = new EventEmitter();
  @Output() pageClicked: EventEmitter<any> = new EventEmitter();
  @ViewChildren("tcaRows") tcaRows: QueryList<ElementRef>;

  constructor(
    private fb: FormBuilder,
    private changeDetector: ChangeDetectorRef,
    private accountService: AccountService,
    private zone: NgZone,
    private DsPopup: DsPopupService,
    private clockService: ClockService,
    private sanitizer: DomSanitizer,
    private dialog: MatDialog,
    private appConfig: AppConfig
  ) {}
  ngAfterViewChecked(): void {
    if (this.clockService.selectedRow)
      this.focusCompComment(this.clockService.selectedRow, true);
  }

  ngOnInit() {
    forkJoin(
      this.accountService
        .canPerformActions(ACTION_TYPES.CLIENTRATE_VIEWHOURLYRATES)
        .pipe(map((result) => result === true)),
      this.accountService
        .canPerformActions(ACTION_TYPES.CLIENTRATE_VIEWSALARYRATES)
        .pipe(map((result) => result === true))
    ).subscribe(([viewHourly, viewSalary]) => {
      this.readRate = viewHourly || viewSalary;
      this.changeDetector.detectChanges();
    });

    this.ApproveAllCheckboxesControl.valueChanges
      .pipe(
        tap((x) => this.toggleAllApprovedInputs(x)),
        takeUntil(this.unsubscriber)
      )
      .subscribe();
    (window as any).handleClose = () => this.handleClose();
  }

  ngOnDestroy(): void {
    if (this.listenToForm) this.listenToForm.unsubscribe();
    this.unsubscriber.next();
  }

  focusCompComment(idx: number, scrollTo = false) {
    if (scrollTo)
      if (this.tcaRows)
        if (
          this.tcaRows.find((el, i) => i === idx).nativeElement &&
          this.clockService.modalClosing
        ) {
          this.tcaRows
            .find((el, i) => i === idx)
            .nativeElement.scrollIntoView({ block: "center" });
          this.clockService.selectedRow = 0;
          this.clockService.modalClosing = false;
        }
  }

  openEditScheduleModal(
    scheduleId: number,
    eventDate: string,
    employeeId: number,
    rowId: number
  ) {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    const url = encodeURI(`&ClockEmployeeScheduleID=${this.empyStringWhenNull(
      scheduleId
    )}
  &EventDate=${this.empyStringWhenNull(
    eventDate
  )}&EmployeeID=${this.empyStringWhenNull(employeeId)}`);
    this.openModal(
      "ModalContainer.aspx?URL=ClockEmployeeScheduleEdit.aspx" + url
    );
  }

  openAddPunchModal(
    addDate: string,
    employeeId: number,
    rowId: number,
    addPunch: string
  ) {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    if (addPunch) {
      this.openModal(addPunch.replace(/'/g, ""));
    } else {
      const url = encodeURI(
        `&AddDate=${this.empyStringWhenNull(
          addDate
        )}&EmployeeID=${this.empyStringWhenNull(employeeId)}`
      );
      this.openModal(
        "ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx" + url
      );
    }
  }

  determinePunchModalMode(
    punchId: number,
    employeeId: number,
    displayText: string,
    eventDate: string,
    rowId: number
  ) {
    if (punchId && punchId > 0) {
      this.clockService
        .getPunch(punchId, employeeId)
        .subscribe((punchResultId) => {
          if (punchResultId && punchResultId > 0) {
            this.openEditPunchModal(
              punchId,
              employeeId,
              displayText,
              eventDate,
              rowId
            );
          } else {
            this.openPendingModalApproval(
              punchId,
              employeeId,
              displayText,
              eventDate,
              rowId
            );
          }
        });
    } else {
      this.openEditPunchModal(
        punchId,
        employeeId,
        displayText,
        eventDate,
        rowId
      );
    }
  }

  openPendingModalApproval(
    punchId: number,
    employeeId: number,
    displayText: string,
    eventDate: string,
    rowId: number
  ) {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    this.selectedPunch = punchId + "";
    const params = `${encodeURI(
      `&EmployeeID=${employeeId}&PunchDate=${eventDate}&PunchRequestID=${punchId}`
    )}`;
    this.openModal(
      `ModalContainer.aspx?URL=ClockEmployeePunchRequest.aspx${params}`
    );
  }

  openEditPunchModal(
    punchId: number,
    employeeId: number,
    displayText: string,
    eventDate: string,
    rowId: number
  ) {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    this.selectedPunch = punchId + "";
    let pwd = punchId == 0 ? "" : "&Password=ZSJ569OPL2N8HG";
    if ("Missing".localeCompare(displayText) === 0) {
      pwd += "&AddDate=" + this.empyStringWhenNull(eventDate);
    }
    const url = encodeURI(`&ClockEmployeePunchID=${this.empyStringWhenNull(
      punchId
    )}
  &EmployeeID=${this.empyStringWhenNull(employeeId)}${pwd}&StopPostback=1`);

    this.openModal("ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx" + url);
  }

  openClockEmployeeBenefitEdit(
    addDate: string,
    employeeId: number,
    rowId: number,
    addBenefit: string
  ) {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    if (addBenefit) {
      this.openModal(addBenefit.replace(/'/g, ""));
    } else {
      const url = encodeURI(
        `&AddDate=${this.empyStringWhenNull(
          addDate
        )}&EmployeeID=${this.empyStringWhenNull(employeeId)}`
      );
      this.openModal(
        "ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx" + url
      );
    }
  }

  openAllocateHours(
    eventDate: string,
    employeeId: number,
    rowId: number
  ): void {
    this.selectedRow = rowId;
    this.clockService.selectedRow = rowId;
    const url = encodeURI(
      `&EventDate=${this.empyStringWhenNull(
        eventDate
      )}&EmployeeID=${this.empyStringWhenNull(employeeId)}`
    );
    this.openModal(
      "ModalContainer.aspx?URL=ClockEmployeeAllocateHours.aspx" + url
    );
  }

  openModal(url: String) {
    const w = window,
      d = document,
      e = d.documentElement,
      g = d.getElementsByTagName("body")[0],
      x = 500,
      y = 550,
      xt = w.innerWidth || e.clientWidth || g.clientWidth,
      yt = w.innerHeight || e.clientHeight || g.clientHeight;

    const left = (xt - x) / 2;
    const top = (yt - y) / 4;

    this.accountService.getSiteUrls()
      .subscribe(sites => {
        const baseUrl = sites.find(s => s.siteType == ConfigUrlType.Payroll);
        url = `${baseUrl.url}${url}`;
        const modal = this.DsPopup.open(url, "_blank", {
          height: y,
          width: x,
          top: top,
          left: left,
        });
      });
  }

  alertUser(content: string): void {
    alert(content);
  }

  // THIS SHOULD BE MOVED TO THE COMPONENT, PASS IN THE EMPLOYEE ID AND OPEN IT
  // THAT WAY THERE IS NO WEIRD DELAY WHEN THEY OPEN THE DIALOGUE
  // OR THIS NEEDS TO BE DONE EARLIER IN THE LIFE CYCLE
  openPunchMapModal(employeeId: string, clientId: string): void {
    const res: EmployeePunchData = this.getEmployeePunchIDList(employeeId);

    this.buildPunchMapModal(employeeId, clientId, res);
  }

  buildPunchMapModal(
    employeeId: string,
    clientId: string,
    employeePunchData: EmployeePunchData
  ): MatDialogRef<PunchMapModalComponent, any> {
    // I WOULD CHANGE PUNCHMAPINFO TO BE THE EMPLOYEE ID
    // THEN JUST LOAD THE REST FROM THE DIALOG

    const modalInstance = this.dialog.open(PunchMapModalComponent, {
      width: "800px",
      height: "800px",
      data: {
        employeeId: employeeId,
        clientId: clientId,
        employeePunchData: employeePunchData,
      },
    });

    modalInstance.afterClosed().subscribe((result) => {
      if (result == null) return;
    });

    return modalInstance;
  }

  getEmployeePunchIDList(employeeID: string): EmployeePunchData {
    const punchIdList = [];
    let name = "";

    this.tableData.table.forEach((x) => {
      if (x.employeeID === employeeID) {
        if (x.header && x.header.toUpperCase() === "HEADER".toUpperCase()) {
          name = x.day;
        }
        if (x.inEmployeePunchID) {
          punchIdList.push(Number(x.inEmployeePunchID));
        }
        if (x.in2EmployeePunchID) {
          punchIdList.push(Number(x.in2EmployeePunchID));
        }
        if (x.outEmployeePunchID) {
          punchIdList.push(Number(x.outEmployeePunchID));
        }
        if (x.out2EmployeePunchID) {
          punchIdList.push(Number(x.out2EmployeePunchID));
        }

        this.setEmployeePunchDates(x.eventDate);
      }
    });

    const res: EmployeePunchData = {
      name: name,
      punchIdList: punchIdList,
    };

    return res;
  }

  setEmployeePunchDates(eventDate: string): void {
    if (
      !this.employeePunchStartDate ||
      moment(eventDate, this.vbDate) < this.employeePunchStartDate
    ) {
      this.employeePunchStartDate = moment(eventDate, this.vbDate);
    }
    if (
      !this.employeePunchEndDate ||
      moment(eventDate, this.vbDate) > this.employeePunchEndDate
    ) {
      this.employeePunchEndDate = moment(eventDate, this.vbDate);
    }
  }

  private emitPopUpClosed(): void {
    this.clockService.modalClosing = true;
    this.popUpClosed.emit(null);
  }

  toggleAllApprovedInputs(newVal: any): void {
    const controls = Object.keys(this.approvedCheckboxes.controls);
    controls.forEach((x) => {
      if (!this.approvedCheckboxes.controls[x].value.selectHoursDisabled) {
        (
          this.approvedCheckboxes.controls[x] as FormGroup
        ).controls.isApproved.setValue(!!newVal);
      }
    });
  }

  loadPage(page: number): void {
    this.ApproveAllCheckboxesControl.setValue(false);
    this.pageClicked.emit(page);
    window.scrollTo(0, 0);
  }

  /**
   * Gets a form with the controls for all of the approval checkboxes on the table
   */
  private createForm(dataSet: Maybe<DataRow[]>): FormGroup {
    let streams: Observable<any>[] = [];
    if (this.listenToForm) this.listenToForm.unsubscribe();

    this.approvedCheckboxes = dataSet
      .map((x) =>
        x.filter(
          (y) =>
            !y.hideApprovalCheckBox ||
            (!("HEADER".localeCompare(y.header) === 0) &&
              !y.isTotalRow &&
              y.schedule)
        )
      )
      .map((x) =>
        x.reduce((prev, next) => {
          const currentRow = this.createFormForRow(next);
          streams = streams.concat(currentRow.streams);
          prev[next.id] = currentRow.group;

          return prev;
        }, [])
      )
      .map((x) => this.fb.group(x))
      .valueOr(this.fb.group({}));

    this.listenToForm = merge(
      this.approvedCheckboxes.valueChanges.pipe(
        startWith(this.approvedCheckboxes.value),
        filter(() => this.approvedCheckboxes.valid),
        tap((x) => this.formValue.emit(x))
      ),
      merge(...streams)
    ).subscribe();
    return this.approvedCheckboxes;
  }

  private createFormForRow(row: DataRow): {
    group: FormGroup;
    streams: Observable<any>[];
  } {
    const isApproved = this.fb.control(row.selectHoursChecked);
    const clockclientNoteID = this.fb.control(row.setClockClientNoteID);
    const changed: Observable<any>[] = [];
    if (row.selectHoursDisabled) isApproved.disable();
    if (row.setClockClientNoteDisabled) clockclientNoteID.disable();

    changed.push(this.emitWhenChanged(isApproved, this.changedControls, true));
    changed.push(this.emitWhenChanged(clockclientNoteID, this.changedControls));
    return {
      group: this.fb.group({
        Date: this.fb.control(
          "".localeCompare(row.clientCostCenterID) === 0 ? "1/1/1900" : row.date
        ),
        ID: this.fb.control(row.id),
        employeeId: this.fb.control(row.employeeID),
        eventDate: this.fb.control(row.eventDate),
        isApproved: isApproved,
        isApprovedVisible: this.fb.control(row.selectHoursShowing),
        clientCostCenterID: this.fb.control(row.clientCostCenterID),
        clockclientNoteID: clockclientNoteID,
        clockclientNoteIDOrig: this.fb.control(row.setClockClientNoteID),
        chkPayToSchedule: this.fb.control(row.payToSchedule),
        // 'disabled': this.fb.control(row.selectHoursDisabled),
        selectHoursDisabled: this.fb.control(row.selectHoursDisabled),
        setClockClientNoteDisabled: this.fb.control(
          row.setClockClientNoteDisabled
        ),
      }),
      streams: changed,
    };
  }

  // private updateViewWhenANoteChanges(form: FormGroup): Observable<any> {
  //     return form.controls['clockclientNoteID'].valueChanges.pipe(
  //         tap(x => this.changeDetector.markForCheck())
  //     );
  // }

  emitWhenChanged(
    control: FormControl,
    changed: boolean[],
    isApprovalCheckbox = false
  ): Observable<any> {
    const origValue = control.value;
    changed.push(false);
    const controlLocation = changed.length - 1;
    return control.valueChanges.pipe(
      tap((value) => {
        if (isApprovalCheckbox) this.isApprovalChangedPipe(value, control);
      }),
      map((newVal) => origValue !== newVal),
      tap((isDifferent) => {
        changed[controlLocation] = isDifferent;
      })
    );
  }

  isApprovalChangedPipe(value: boolean, control: FormControl): void {
    const selectedCostCenterId = control.parent.value.clientCostCenterID;
    if (!selectedCostCenterId) return;
    const group = control.parent.parent;
    const dt = moment(control.parent.value.eventDate, this.vbDate);
    const employeeId = +control.parent.value.employeeId;

    const sameCostCenters = [];
    for (const rowId in group.value) {
      const row = group.value[rowId];
      const eventDate = moment(row.eventDate, this.vbDate);

      // if employee, event or cost center aren't the same we aren't going to update the value of the checkbox on them...
      if (
        +row.employeeId !== employeeId ||
        !dt.isSame(eventDate) ||
        +selectedCostCenterId !== +row.clientCostCenterID
      )
        continue;

      sameCostCenters.push(rowId);
    }

    sameCostCenters.forEach((id) => {
      group.get(`${id}.isApproved`).setValue(value, { emitEvent: false });
    });
  }

  goToNextPage(): void {
    this.pageClicked.emit(this.tableData.currentPage + 1);
    window.scrollTo(0, 0);
  }

  goToPreviousPage(): void {
    this.pageClicked.emit(this.tableData.currentPage - 1);
    window.scrollTo(0, 0);
  }

  private readonly empyStringWhenNull = (val: any) =>
    new Maybe(val).valueOr("");

  // compareFn(value1: string, value2: string): boolean {
  //     const first = +value1;
  //     const second = +value2;
  //     if (isNaN(first) && isNaN(second))
  //         return true;
  //     return new Maybe(first).map(x => new Maybe(second).map(y => x === y).value()).valueOr(false);
  // }

  /**
   * @name handleClose
   * @param {String} retVal - Value returned by the popup.
   * @description
   * Called by a closing popup window.
   */
  handleClose() {
    this.zone.run(() => {
      this.emitPopUpClosed();
    });
  }

  // THIS IS NOT USED AND CAN BE REMOVED
  // UpdateDataGridRow(myString: string, rowToEdit: FormGroup, realRowToEdit: DataRow) {
  //     const mySplitResult = myString.split('¿');
  //     const mypunch = mySplitResult[1];
  //     const myhours = mySplitResult[2];
  //     let mynotes = mySplitResult[3];
  //     const myexceptions = mySplitResult[4];
  //     realRowToEdit.exceptions = myexceptions;
  //     realRowToEdit.hours = myhours;

  //     if (mynotes !== null) {
  //         mynotes = this.removeBreaksFromStrings(mynotes);
  //     }

  //     if (this.selectedPunch !== null) {
  //         if (this.selectedPunch === realRowToEdit.inEmployeePunchID) {
  //             realRowToEdit.in = mypunch;
  //         } else if (this.selectedPunch === realRowToEdit.outEmployeePunchID) {
  //             realRowToEdit.out = mypunch;
  //         } else if (this.selectedPunch === realRowToEdit.in2EmployeePunchID) {
  //             realRowToEdit.in2 = mypunch;
  //         } else if (this.selectedPunch === realRowToEdit.out2EmployeePunchID) {
  //             realRowToEdit.out2 = mypunch;
  //         }
  //     }

  //     this.selectedPunch = null;

  //     realRowToEdit.notes = mynotes;

  //     const myChildNode = 0;
  //     if (myString.indexOf('ALLOCATEHOURS') !== -1) {
  //         rowToEdit.controls['isApproved'].setValue(false);
  //         let myAllocateColor = 'red';
  //         if (myString === 'ALLOCATEHOURSCHECKED') {
  //             myAllocateColor = 'green';
  //             rowToEdit.controls['isApproved'].setValue(true);
  //         }
  //     } else if (myString.indexOf('DELETE') !== -1) {
  //         this.emitPopUpClosed();
  //     } if (mySplitResult[0] === 'PUNCH' || mySplitResult[0] === 'BENEFIT') {
  //         realRowToEdit.isApproved = false;
  //         let location = 0;
  //         let start = 0;
  //         start = location = this.tableData.table.indexOf(realRowToEdit);
  //         let current = this.tableData.table[location];
  //         while (current != null) {
  //             const isDayOfWeek = this.isDayOfWeek(current.day);

  //             if (isDayOfWeek) {
  //                 break;
  //             } else {
  //                 location--;
  //                 current = this.tableData.table[location];
  //             }
  //         }
  //         start = location;
  //         while (current != null) {
  //             const isDayRow = this.isDayOfWeek(current.day);

  //             const isNextDayRow = isDayRow && location !== start;

  //             if (location < this.tableData.table.length - 1 && !isNextDayRow) {
  //                 const isApprovedCtrl = new Maybe(this.approvedCheckboxes.get(`${current.id}`) as FormGroup)
  //                     .map(x => x.get('isApproved')).value();

  //                 if (isApprovedCtrl) {
  //                     isApprovedCtrl.setValue(false);
  //                 }
  //                 location++;
  //                 current = this.tableData.table[location];
  //             } else {
  //                 current = null;
  //                 continue;
  //             }
  //         }
  //     }

  //     function clearCheckbox(checkboxRow) {
  //         rowToEdit.controls['isApproved'].setValue(false);
  //     }

  //     function clearCell(cell) {
  //         if (cell) {
  //             const node = cell.childNodes[myChildNode];
  //             if (node) {
  //                 node.innerHTML = '';
  //             }
  //         }
  //     }

  // }

  // isDayOfWeek(day: string): boolean {
  //     return new Maybe(day)
  //         .map(x => x.toUpperCase())
  //         .map(x => {
  //             return this.daysOfWeek
  //                 .map(y => y.toUpperCase())
  //                 .some(validDayString => validDayString === x);
  //         })
  //         .valueOr(false);
  // }

  removeBreaksFromStrings(inputStr: string) {
    let i = 0;
    let outputStr = "";

    if (!inputStr) return;
    const containsBr: boolean = inputStr.includes("<br>");
    let inputStrArray: string[];
    let tmpString: string;
    tmpString = inputStr.replace(/<b>/g, "");
    tmpString = tmpString.replace(/<\/b>/g, "");
    if (containsBr) {
      inputStrArray = tmpString.split("<br>");
    } else {
      inputStrArray = tmpString.split("<br/>");
    }

    if (inputStrArray.length > 0) {
      inputStrArray.forEach((item) => {
        if (i > 0) {
          outputStr = outputStr + "\n" + item;
        } else {
          outputStr = item;
        }
        i++;
      });
      i = 0;
    } else {
      outputStr = inputStrArray[0];
    }

    return outputStr;
  }

  getOptions(notes: GetClockClientNoteListResultDto[], row: DataRow) {
    return notes.filter(
      (x) =>
        x.clockClientNoteID.toString() === row.clockClientNoteID || x.isActive
    );
  }

  // checkIsMissingPunch(row: DataRow): boolean {
  //     const isMissingPunch = this._normalizeValue(row.exceptions).includes('missing punch');
  //     if (!isMissingPunch) return !isMissingPunch;
  //     return !(row.inEmployeePunchID > '0' && row.outEmployeePunchID > '0')
  //         || !(row.in2EmployeePunchID > '0' && row.out2EmployeePunchID > '0');
  // }

  checkIsMissingPunch(row: DataRow): boolean {
    // CHECK THAT THE ONLY EXCEPTION ISNT A MISSING PUNCH
    // THIS IS A "NICE TO HAVE" SO IT MAY FAIL
    const isMissingPunch = this._normalizeValue(row.exceptions).startsWith(
      "missing punch"
    );
    if (!isMissingPunch) return !isMissingPunch;
    // CONVERT PUNCH ID FROM STRING TO VALUE
    const inPunchId = coerceNumberProperty(row.inEmployeePunchID);
    const outPunchId = coerceNumberProperty(row.outEmployeePunchID);
    // iF THE FIRST PUNCH PAIR IS MISSING AN ID SHOW EXCEPTIONS
    if (!(inPunchId > 0 && outPunchId > 0)) return true;
    // CONVERT NEXT PAIR OF PUNCH ID TO VALUE
    const in2PunchId = coerceNumberProperty(row.in2EmployeePunchID);
    const out2PunchId = coerceNumberProperty(row.out2EmployeePunchID);
    // IF PUNCH PAIR IS MISSING A PUNCH SHOW THE EXCEPTIONS
    if (!(in2PunchId > 0 && out2PunchId > 0)) return true;
    // WE DIDN'T SEE ANY MISSING PUNCHES SO HIDE EXCEPTIONS
    return false;
  }

  getCostCentersDoNotMatchError(row: DataRow): string {
    let msg = '<div class="pt-2">Cost Centers do not match: </div>';
    let sendMsg = false;
    const noCCLabel = "[None Selected]";
    if (+row.inEmployeePunchID && +row.outEmployeePunchID) {
      const costCentersMatch = this._checkStringMatches(
        row.inCostCenterDesc,
        row.outCostCenterDesc
      );
      if (!costCentersMatch) {
        sendMsg = true;
        msg += `<div class="py-1"><div>#1 ${
          row.inCostCenterDesc || noCCLabel
        }</div>
                    <div>#2 ${row.outCostCenterDesc || noCCLabel}</div></div>`;
      }
    }

    if (+row.in2EmployeePunchID && +row.out2EmployeePunchID) {
      const costCenters2Match = this._checkStringMatches(
        row.in2CostCenterDesc,
        row.out2CostCenterDesc
      );
      if (!costCenters2Match) {
        sendMsg = true;
        msg += `<div class="py-1"><div>#3 ${
          row.in2CostCenterDesc || noCCLabel
        }</div>
                    <div>#4 ${row.out2CostCenterDesc || noCCLabel}</div></div>`;
      }
    }

    return sendMsg
      ? (this.sanitizer.bypassSecurityTrustHtml(msg) as string)
      : "";
  }

  // getSafeHtml(value: string): SafeHtml {
  //     return this.sanitizer.bypassSecurityTrustHtml(value);
  // }

  private _checkStringMatches(one: string, two: string): boolean {
    return this._normalizeValue(one) === this._normalizeValue(two);
  }

  private _normalizeValue(value: string): string {
    return value ? value.trim().toLowerCase() : "";
  }
}
