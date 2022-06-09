import {
  Directive,
  forwardRef,
  ElementRef,
  HostListener,
  Input,
  Renderer2,
} from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import * as moment from "moment";

@Directive({
  selector: "[dsMomentTimeInput]",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MomentTimeInputDirective),
      multi: true,
    },
  ],
})
export class MomentTimeInputDirective implements ControlValueAccessor {
  private _parseFormats = [
    "h",
    "ha",
    "h a",
    "h:mm",
    "h:mm a",
    "h:mma",
    "hmm",
    "hmma",
    "hmm a",
  ];
  private _displayFormat = "hh:mm A";

  private _lastMoment: moment.Moment;

  // control value accessor methods
  @HostListener("input", ["$event.target.value"]) onChange: any = (
    _: any
  ) => {};
  @HostListener("blur", []) onTouched: any = () => {};
  @Input() momentMeridiemDefault: string;
  @Input() baseDate: moment.Moment | Date | string;
  disabled: boolean = false;

  constructor(
    private _renderer: Renderer2,
    private _elementRef: ElementRef<HTMLInputElement>
  ) {}

  @HostListener("blur", [])
  onBlur() {
    if (this._lastMoment && this._lastMoment.isValid()) {
      this.renderViewValue(this._lastMoment.format(this._displayFormat));
    } else {
      this.renderViewValue("");
    }
  }

  writeValue(vDate: any): void {
    if (vDate) {
      let m = moment(vDate);
      m.isValid
        ? this.renderViewValue(m.format(this._displayFormat))
        : this.renderViewValue("");
      this._lastMoment = m;
    } else {
      this.renderViewValue("");
    }
  }
  registerOnChange(fn: any): void {
    this.onChange = (viewValue: string) => {
      if (viewValue.trim() === "") {
        this._lastMoment = null;
        fn(null);
      }

      let date: moment.Moment,
        nonColonTime = viewValue ? viewValue.match(/\b\d{3}\D*\b/) : null,
        meridiemMatch;

      // ensure values such as '830' are parsed properly by moment
      // basically, moment requires a 4-digit value when matching on 'hmm', so always force 4-digits
      if (nonColonTime) {
        viewValue = viewValue.replace(nonColonTime[0], "0" + nonColonTime[0]);
      }

      // set default AM / PM if specified
      if (this.momentMeridiemDefault && viewValue) {
        meridiemMatch = viewValue.match(/[aApP][mM]?/);

        // if no match then AM/PM was not specified, so add the default
        if (!meridiemMatch) {
          viewValue += this.momentMeridiemDefault;
        }
      }

      date = moment(viewValue, this._parseFormats, true); // last param forces strict parsing

      if (date.isValid()) {
        if (this.baseDate) {
          let mBase = moment(this.baseDate);
          date = date.set({
            date: mBase.get("date"),
            month: mBase.get("month"),
            year: mBase.get("year"),
          });
        }
        fn(date);
        this._lastMoment = date;
      } else {
        fn(null);
        this._lastMoment = null;
      }
    };
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  private renderViewValue(val: string) {
    this._renderer.setProperty(this._elementRef.nativeElement, "value", val);
  }
}
