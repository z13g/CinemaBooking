import { Area } from "../area/area";

export class Region
{ 
  regionID?: number
  regionName: string
  areas?: Area[];

  constructor(regionID: number, regionName: string) {
    this.regionID = regionID;
    this.regionName = regionName
  }
}
