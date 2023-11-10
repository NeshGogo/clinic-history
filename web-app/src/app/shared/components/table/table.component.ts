import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table class="w-full text-sm text-left text-gray-500 dark:text-gray-400">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
          <tr>
            @for (header of headers; track $index) {
            <th scope="col" class="px-6 py-3">
              {{ header }}
            </th>
            }
          </tr>
        </thead>
        <tbody>
          <ng-content></ng-content>
        </tbody>
      </table>
    </div>
  `,
})
export class TableComponent {
  @Input() headers: string[] = [];
  @Input() items: unknown[] = [];
}
