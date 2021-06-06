import { computeDecimalDigest } from "@angular/compiler/src/i18n/digest";

export class Search{
 code : number
 userId? : number 
 myLocationAddress : string 
 place_id : string 
 cityCode? : number
 sizeOpt? : boolean
 preferableWidth? : number
 preferableLength? : number
 roofOpt? : boolean
 daysSchedule? : number
 minPrice? : number
 maxPrice? : number
 regularly? : boolean
 searchDate ? : Date
}