import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  CardBodyComponent,
  RowComponent,
  ColComponent,
  CardHeaderComponent,
  CardComponent,
  FormControlDirective,
  ButtonDirective,
  TableDirective,
  BadgeComponent,
  ModalComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ButtonCloseDirective,
  ModalBodyComponent,
  ModalFooterComponent,
  FormLabelDirective,
  FormSelectDirective,
  AlertComponent,
} from '@coreui/angular';

import {
  FeedbackApiClient,
  FeedbackDto,
  AdminReplyDto,
  FeedbackStatus,
} from '../../../api/admin-api.service.generated';

@Component({
  standalone: true,
  selector: 'app-feedback',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardBodyComponent,
    RowComponent,
    ColComponent,
    CardHeaderComponent,
    CardComponent,
    FormControlDirective,
    ButtonDirective,
    TableDirective,
    BadgeComponent,
    FormLabelDirective,
    ModalComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    FormSelectDirective,
    ButtonCloseDirective,
    ModalBodyComponent,
    ModalFooterComponent,
    AlertComponent,
  ],
  templateUrl: './feedback.component.html',
})
export class FeedbackComponent implements OnInit {
  feedbacks: FeedbackDto[] = [];
  isLoading = false;
  visible = false;
  selectedFeedback?: FeedbackDto;
  replyForm!: FormGroup;

  private feedbackApi = inject(FeedbackApiClient);
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.initForm();
    this.loadFeedbacks();
  }

  initForm() {
    this.replyForm = this.fb.group({
      replyContent: ['', [Validators.required, Validators.minLength(5)]],
      newStatus: [FeedbackStatus.Resolved, Validators.required],
    });
  }

  loadFeedbacks() {
    this.isLoading = true;
    this.feedbackApi.getAll().subscribe({
      next: (data) => {
        this.feedbacks = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Lỗi tải phản hồi:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  openReplyModal(item: FeedbackDto) {
    this.selectedFeedback = item;
    this.replyForm.patchValue({
      replyContent: item.adminReply || '',
      newStatus: item.status ?? FeedbackStatus.Resolved,
    });
    this.visible = true;
  }

  onSubmitReply() {
    if (this.replyForm.invalid || !this.selectedFeedback) {
      return;
    }

    this.isLoading = true;
    const val = this.replyForm.value;
    const dto = new AdminReplyDto();
    dto.replyContent = val.replyContent;
    dto.newStatus = Number(val.newStatus);

    this.feedbackApi.reply(this.selectedFeedback.id!, dto).subscribe({
      next: () => {
        alert('Gửi phản hồi thành công!');
        this.visible = false;
        this.loadFeedbacks();
      },
      error: (err) => {
        this.isLoading = false;
        alert('Lỗi khi gửi phản hồi');
        console.error(err);
      },
    });
  }

  getStatusColor(status: any): string {
    switch (status) {
      case FeedbackStatus.Resolved:
        return 'success';
      case FeedbackStatus.Processing:
        return 'info';
      default:
        return 'warning';
    }
  }

  getStatusText(status: any): string {
    switch (status) {
      case FeedbackStatus.Resolved:
        return 'Đã giải quyết';
      case FeedbackStatus.Processing:
        return 'Đang xử lý';
      default:
        return 'Chờ xử lý';
    }
  }
}
