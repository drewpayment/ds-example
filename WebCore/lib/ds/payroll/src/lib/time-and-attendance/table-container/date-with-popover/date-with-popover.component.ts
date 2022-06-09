import {
  Component,
  Input,
  ChangeDetectionStrategy,
  EventEmitter,
  Output,
} from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { DataRow } from '../../shared/DataRow.model';

@Component({
  selector: 'ds-date-with-popover',
  templateUrl: './date-with-popover.component.html',
  styleUrls: ['./date-with-popover.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DateWithPopoverComponent {
  constructor(private accountService: AccountService) {}

  @Input() row: DataRow;

  @Output() popUpClosed: EventEmitter<any> = new EventEmitter();
  @Output() rowClickedEmitter: EventEmitter<number> = new EventEmitter();

  rowClicked() {
    this.rowClickedEmitter.emit(this.row.id);
    // PREVIOUSLY WE WERE RETURNING THIS STRING WITH THE JAVASCRIPT OPEN MODAL METHOD WRAPPING OUR NEEDED VALUES.
    // WE DONT USE THIS IN ANGULAR SO I ANALYZED THE STRING BEING RETURNED AND SPLIT IT ON THE ' VALUE, THIS
    // APPROPRIATELY BROKE OFF THE JAVASCRIPT OPEN MODAL. FROM HERE I USED THE REMAINDER SPLITS TO BUILD THE
    // SIZING STRING WHICH WAS ALSO BEING BROKEN UP BY THE ' VALUE.
    // IF STUFF BREAKS DOWN THE LINE CONVERT MODAL TO ANGULAR. IF NOT ENOUGH TIME LOOK INTO THE SUBSTRING BREAK
    // THAT I HAVE COMMENTED OUT.
    // COLLIN B 12/18/2019
    // https://img.izismile.com/img/img4/20111209/640/cannonball_fail_640_22.jpg
    var res: string[] = this.row.lnkDateModal.split("'");

    this.openModal(res[1]);
  }
  openModal(url: string) {
    const w = window,
      d = document,
      e = d.documentElement,
      g = d.getElementsByTagName('body')[0],
      x = 500,
      y = 550,
      xt = w.innerWidth || e.clientWidth || g.clientWidth,
      yt = w.innerHeight || e.clientHeight || g.clientHeight;

    const left = (xt - x) / 2;
    const top = (yt - y) / 4;

    this.accountService
      .getSiteConfig(ConfigUrlType.Payroll)
      .subscribe((site) => {
        url = `${site.url}${url}`;
        window.open(
          url,
          '_blank',
          `left=${left},top=${top},width=${x},height=${y};`
        ).onbeforeunload = () => {
          this.emitPopUpClosed();
        };
      });
  }

  private emitPopUpClosed(): void {
    this.popUpClosed.emit(null);
  }
}
