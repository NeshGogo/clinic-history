import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LinkItem } from 'src/app/core/models/linkItem';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  @Input() items: LinkItem[] = [];
  @Input() open: boolean = false;
  @Output() openChange: EventEmitter<boolean> = new EventEmitter(false);

  close(){
    this.open = false;
    this.openChange.emit(this.open);
  }
}
