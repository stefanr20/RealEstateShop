import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Property } from '../../property';

@Component({
  selector: '[app-property-row]',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-property-row.component.html',
  styleUrls: ['./admin-property-row.component.css']
})
export class AdminPropertyRowComponent {
  @Input() model!: Property;
  @Output() editProperty = new EventEmitter<Property>();
  @Output() deleteProperty = new EventEmitter<number>();

  onEdit() {
    this.editProperty.emit(this.model);
  }

  onDelete() {
    this.deleteProperty.emit(this.model.id);
  }
}