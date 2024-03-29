﻿import { Address } from './address';

export interface Person {
    personId: number;
    firstName: string;
    middleName: string;
    lastName: string;
    profileImage: string;
    age: number;
    address: Address;
    interests: string[];
}
