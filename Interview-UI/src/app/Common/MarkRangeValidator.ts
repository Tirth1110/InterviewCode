import { AbstractControl, ValidatorFn } from '@angular/forms';

export default class Validation {

  // Custom validator to check if the mark is within the range of 0 to 100
  public static markRangeValidator(control: AbstractControl): any {
    if (control.value == undefined || control.value == null) {
      return;
    }
    const mark = control.value;
    return (mark >= 0 && mark <= 100) ? '' : { invalidMarkRange: true };
  }
}
