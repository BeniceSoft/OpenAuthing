import { CheckIcon, XIcon } from "lucide-react"
import { HSStrongPassword } from "preline/preline"
import { HTMLAttributes, HTMLInputTypeAttribute, useEffect, useState } from "react"
import { FieldErrors, UseFormRegister } from "react-hook-form"
import { FormattedMessage, useIntl } from "umi"
import { Input, InputLabel } from "../ui/input"


type PasswordRule = {
    ruleText: string,
    desc: string,
    validate: (value: string) => string | undefined
}


type PasswordInputProps = {
    name: string
    invalid?: boolean
    register: UseFormRegister<any>
} & React.InputHTMLAttributes<HTMLInputElement>

export default ({ register, invalid, name, ...props }: PasswordInputProps) => {
    const intl = useIntl()
    const [rules, setRules] = useState<PasswordRule[]>([])

    const id = props.id ?? 'password'

    const Validations = {
        lengh: (value: string, minLen: number) => {
            if (value.length < minLen) {
                return intl.formatMessage({ id: 'common.password.rule.length' })
            }
        },

        lowercase: (value: string) => {
            if (!/[a-z]/.test(value)) {
                return intl.formatMessage({ id: 'common.password.rule.lowercase' })
            }
        },

        uppercase: (value: string) => {
            if (!/[A-Z]/.test(value)) {
                return intl.formatMessage({ id: 'common.password.rule.uppercase' })
            }
        },

        numbers: (value: string) => {
            if (!/\d/.test(value)) {
                return intl.formatMessage({ id: 'common.password.rule.numbers' })
            }
        },
        specialchars: (value: string, specialCharacters: string) => {
            var regex = new RegExp('[' + specialCharacters.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + ']');
            if (!regex.test(value)) {
                return intl.formatMessage({ id: 'common.password.rule.specialchar' })
            }
        }
    }

    useEffect(() => {
        setRules([
            { ruleText: 'min-length', desc: intl.formatMessage({ id: 'common.password.rule.length' }), validate: value => Validations.lengh(value, 6) },
            { ruleText: 'lowercase', desc: intl.formatMessage({ id: 'common.password.rule.lowercase' }), validate: Validations.lowercase },
            { ruleText: 'uppercase', desc: intl.formatMessage({ id: 'common.password.rule.uppercase' }), validate: Validations.uppercase },
            { ruleText: 'numbers', desc: intl.formatMessage({ id: 'common.password.rule.numbers' }), validate: Validations.numbers },
            { ruleText: 'special-characters', desc: intl.formatMessage({ id: 'common.password.rule.specialchar' }), validate: value => Validations.specialchars(value, "!\"#$%&'()*+,-./:;<=>?@[\\\\\\]^_`{|}~") },
        ])
    }, [])

    useEffect(() => {
        if (rules.length > 0) {
            HSStrongPassword.autoInit()
        }
    }, [rules])

    const validatePassword = (value: string) => {
        for (const rule of rules) {
            let message = rule.validate(value)
            if (message) {
                return message
            }
        }
    }
    return (
        <div className="relative">
            <Input
                {...props}
                id={id}
                type="password"
                autoComplete="new-password"
                aria-invalid={invalid}
                {...register(name, { required: "Password is required", validate: validatePassword })} />

            <div id="oa-password-strong-popover"
                className="hidden absolute z-10 w-full bg-white shadow-xl rounded-lg p-4 dark:bg-gray-800 dark:border dark:border-gray-700 dark:divide-gray-700">
                <div data-hs-strong-password={`{
                        "target": "#${id}",
                        "hints": "#oa-password-strong-popover",
                        "stripClasses": "hs-strong-password:opacity-100 hs-strong-password-accepted:bg-teal-500 h-2 flex-auto rounded-full bg-blue-500 opacity-50 mx-1",
                        "mode": "popover"
                    }`}
                    className="flex mt-2 -mx-1">
                </div>

                <h4 className="mt-3 text-sm font-semibold text-gray-800 dark:text-white">
                    <FormattedMessage id="common.password.rule" />
                </h4>

                <ul className="space-y-1 text-sm text-gray-500">
                    {rules.map(({ ruleText, desc }, index) => (
                        <li key={ruleText} data-hs-strong-password-hints-rule-text={ruleText} className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                            <span className="hidden" data-check="">
                                <CheckIcon className="w-4 h-4" />
                            </span>
                            <span data-uncheck="">
                                <XIcon className="w-4 h-4" />
                            </span>
                            {desc}
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    )
}