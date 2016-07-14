﻿using System;

namespace RpgCombatKata.Core.Business.Rules {
    public interface Rules {
        Func<T,T> GetFilterFor<T>() where T : class;
        bool CanApplyTo<T>() where T : class;
    }
}