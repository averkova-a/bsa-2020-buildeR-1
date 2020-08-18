import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'timespan',
})
export class TimespanPipe implements PipeTransform {
  // value - timespan in seconds
  transform(value: number): string {
    if (value < 60) {
      return new DatePipe('en-US').transform(value * 1000, "ss 'sec'");
    } else if (value < 60 * 60) {
      if (value % 60 == 0) {
        return new DatePipe('en-US').transform(value * 1000, "mm 'min'");
      }
      return new DatePipe('en-US').transform(value * 1000, "mm 'min' ss 'sec'");
    }
    // ...
  }
}
