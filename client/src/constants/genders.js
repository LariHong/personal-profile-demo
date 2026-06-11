export const GENDER_OPTIONS = [
  { value: 'Male', label: '男' },
  { value: 'Female', label: '女' },
  { value: 'Other', label: '其他' }
];

export function genderText(gender) {
  return GENDER_OPTIONS.find((option) => option.value === gender)?.label ?? gender;
}
