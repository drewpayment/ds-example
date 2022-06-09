import { FormControl, FormGroup } from '@angular/forms';


export class PasswordValidator {
  static areEqual(fg: FormGroup) {
    let value, valid = true;
    for (let key in fg.controls) {
      if (fg.controls.hasOwnProperty(key)) {
        let control: FormControl = fg.controls[key] as FormControl;

        if (value === undefined) {
          value = control.value;
        } else if (value !== control.value) {
          valid = false;
          break;
        }
      }
    }

    if (!!valid) return null;
    return { areEqual: true };
  }
}
