import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-drawer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './drawer.component.html',
})
export class DrawerComponent {
  @Input() title: string = '';
  @Input() direction: 'right' | 'left' = 'right';
  @Input() open: boolean = false;
  @Output() openChange: EventEmitter<boolean> = new EventEmitter(false);
  @Output() onClose = new EventEmitter<void>();
  isRight: boolean = this.direction == 'right';

  close() {
    this.open = false;
    this.openChange.emit(this.open);
    this.onClose.emit();
  }
}
