import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SwalToastService {

  constructor() { }

  toast(title: string, text: string, icon: SweetAlertIcon, showCancelButton: boolean, confirmButtonText: string = '<i class="ri-check-line align-bottom me-1"></i>Done') {
    let timerInterval: any;
    Swal.fire({
      title: title,
      html: text,
      timer: 1500,
      icon: icon,
      showCancelButton: showCancelButton,
      timerProgressBar: true,
      confirmButtonColor: 'rgb(3, 142, 220)',
      cancelButtonColor: 'rgb(243, 78, 78)',
      confirmButtonText: confirmButtonText,
      didOpen: () => {
        timerInterval = setInterval(() => {
          const content = Swal.getHtmlContainer();
          if (content) {
            const b: any = content.querySelector('b');
            if (b) {
              b.textContent = Swal.getTimerLeft();
            }
          }
        }, 100);
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    }).then((result) => {
      if (result.dismiss === Swal.DismissReason.timer) {
      }
    });
  }
}
