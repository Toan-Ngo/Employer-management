import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable()
export class AlertService {
  constructor(private messageService: MessageService) {}
  showSuccess(message: string) {
    this.messageService.add({
      severity: 'success',
      summary: 'Thành Công',
      detail: message,
    });
  }
  showError(err: string) {
    this.messageService.add({ severity: 'Error', summary: 'Lỗi', detail: err });
  }
}
