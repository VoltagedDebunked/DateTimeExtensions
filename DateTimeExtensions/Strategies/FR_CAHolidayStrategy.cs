﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateTimeExtensions.Strategies {
	
	//Canada official languages are french and english.
	// for the time beeing, the en-CA will inherit from fr-CA.
	// in the furure this could be changed...
	public class FR_CAHolidayStrategy : HolidayStrategyBase, IHolidayStrategy {

		public FR_CAHolidayStrategy() {
			this.InnerHolidays.Add(ChristianHolidays.NewYear);
			this.InnerHolidays.Add(ChristianHolidays.GoodFriday);
			this.InnerHolidays.Add(ChristianHolidays.EasterMonday);
			this.InnerHolidays.Add(VictoriaDay);
			this.InnerHolidays.Add(CanadaDay);
			this.InnerHolidays.Add(LabourDay);
			this.InnerHolidays.Add(Thanksgiving);
			this.InnerHolidays.Add(RemembranceDay);
			this.InnerHolidays.Add(ChristianHolidays.Christmas);
			this.InnerHolidays.Add(GlobalHolidays.BoxingDay);
		}

		public override IDictionary<DateTime, Holiday> BuildObservancesMap(int year) {
			IDictionary<DateTime, Holiday> holidayMap = new Dictionary<DateTime, Holiday>();
			foreach (var innerHoliday in InnerHolidays) {
				var date = innerHoliday.GetInstance(year);
				if (date.HasValue) {
					holidayMap.Add(date.Value, innerHoliday);
					//if the holiday is a saturday, the holiday is observed on previous friday
					if (date.Value.DayOfWeek == DayOfWeek.Saturday) {
						holidayMap.Add(date.Value.AddDays(2), innerHoliday);
					}
					//if the holiday is a sunday, the holiday is observed on next monday
					if (date.Value.DayOfWeek == DayOfWeek.Sunday) {
						holidayMap.Add(date.Value.AddDays(1), innerHoliday);
					}
				}
			}
			return holidayMap;
		}

		//First Monday in September - Canada Day
		private static Holiday canadaDay;
		public static Holiday CanadaDay {
			get {
				if (canadaDay == null) {
					canadaDay = new FixedHoliday("Canada Day", 7, 1);
				}
				return canadaDay;
			}
		}

		//First Monday in September - Labour Day
		private static Holiday labourDay;
		public static Holiday LabourDay {
			get {
				if (labourDay == null) {
					labourDay = new NthDayOfWeekInMonthHoliday("Labour Day", 1, DayOfWeek.Monday, 9, CountDirection.FromFirst);
				}
				return labourDay;
			}
		}

		//Monday on or before May 24 - Victoria Day
		private static Holiday victoriaDay;
		public static Holiday VictoriaDay {
			get {
				if (victoriaDay == null) {
					// 25 = day after 24. NthDayOfWeekAfterDayHoliday doesn't count the start day
					victoriaDay = new NthDayOfWeekAfterDayHoliday("Victoria Day", -1, DayOfWeek.Monday, 5, 25);
				}
				return victoriaDay;
			}
		}

		//Second Monday in October - Thanksgiving
		private static Holiday thanksgiving;
		public static Holiday Thanksgiving {
			get {
				if (thanksgiving == null) {
					thanksgiving = new NthDayOfWeekInMonthHoliday("Thanksgiving", 2, DayOfWeek.Monday, 10, CountDirection.FromFirst);
				}
				return thanksgiving;
			}
		}

		//November 11 - Remembrance Day
		private static Holiday remembranceDay;
		public static Holiday RemembranceDay {
			get {
				if (remembranceDay == null) {
					remembranceDay = new FixedHoliday("Remembrance Day", 11, 11);
				}
				return remembranceDay;
			}
		}
	}
}