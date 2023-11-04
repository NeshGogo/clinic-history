import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-disable-enable-zone',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      class="border border-red-700 pt-1 px-1 text-red-600 border-dashed rounded"
    >
      <h6 class="font-bold mb-2">Danger zone</h6>
      <p class="mb-2">
        By {{ isDisabled ? 'enabling' : 'disabling' }} this it will{{
          isDisabled ? ' not' : ''
        }}
        be visible on the platform.
      </p>
      <button
        (click)="onClick()"
        type="submit"
        class="bg-red-400 hover:bg-red-500  text-white w-full focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-md text-sm px-5 py-2.5 text-center mr-2 mb-2"
      >
        {{ isDisabled ? 'Enable' : 'Disable' }}
      </button>
    </div>
  `,
})
export class DisableEnableZoneComponent {
  @Input() isDisabled = false;
  @Output() btnClick = new EventEmitter<void>();

  onClick() {
    this.btnClick.emit();
  }
}
