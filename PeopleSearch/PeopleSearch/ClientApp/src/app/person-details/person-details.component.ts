import { Component, Input } from '@angular/core';
import { Person } from '../models';

@Component({
  selector: 'app-person-details',
  templateUrl: './person-details.component.html',
  styleUrls: ['./person-details.component.css']
})
export class PersonDetailsComponent {
  @Input() person?: Person;
}
