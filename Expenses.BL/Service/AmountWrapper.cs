using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    internal class AmountWrapper : IAmount
    {
        private Func<double> m_getter;

        private Action<double> m_setter;

        public AmountWrapper(Func<double> getter, Action<double> setter) {
            m_setter = setter;
            m_getter = getter;
        }

        public double Amount {
            get {
                return m_getter();
            }
            set {
                m_setter(value);
            }
        }
    }
}

