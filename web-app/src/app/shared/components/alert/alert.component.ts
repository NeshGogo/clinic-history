import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
})
export class AlertComponent {
  @Input() content: string = '';
  @Input() closeable: boolean = false;
  @Input() color: string = 'blue';
  @Output() click: EventEmitter<boolean> = new EventEmitter();

  close(): void{
    if(this.closeable){
      this.click.emit(true);
    }
  }
}
