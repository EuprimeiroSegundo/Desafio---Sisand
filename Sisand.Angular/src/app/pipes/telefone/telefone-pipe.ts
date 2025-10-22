import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'telefone'
})
export class TelefonePipe implements PipeTransform {
  transform(value: string | undefined): string {
    if (!value) {
      return '';
    }

    const telefoneLimpo = value.replace(/\D/g, '');
    const length = telefoneLimpo.length;

    if (length === 10) {
      return `(${telefoneLimpo.substring(0, 2)}) ${telefoneLimpo.substring(2, 6)}-${telefoneLimpo.substring(6, 10)}`;
    }
    
    if (length === 11) {
      return `(${telefoneLimpo.substring(0, 2)}) ${telefoneLimpo.substring(2, 7)}-${telefoneLimpo.substring(7, 11)}`;
    }

    return value;
  }
}
