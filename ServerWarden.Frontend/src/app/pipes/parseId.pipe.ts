import { Pipe, PipeTransform } from '@angular/core';
import { ServerType } from '../models/server';
@Pipe({
  standalone: true,
  name: 'toString'
})
export class ToString implements PipeTransform {
  transform(value: ServerType): string {
    return ServerType[value];
  }
}