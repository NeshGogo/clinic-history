import { Component, Input } from '@angular/core';
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
}
